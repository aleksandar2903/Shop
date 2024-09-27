<?php

use App\Models\User;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Route;
use Illuminate\Support\Facades\URL;

/*
|--------------------------------------------------------------------------
| API Routes
|--------------------------------------------------------------------------
|
| Here is where you can register API routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| is assigned the "api" middleware group. Enjoy building your API!
|
*/

Route::middleware(['auth:sanctum'])->group(function () {
    Route::get('/user', function (Request $request) {
        $user = Auth::user()->load('client')->load(['client_orders' => function ($q) {
            $q->take(5);
        }])->loadCount(['client_orders as delivered_orders_count' => function ($q) {
            $q->where('delivery_status', 'Delivered');
        }])->loadCount(['client_orders as pending_orders_count' => function ($q) {
            $q->where('delivery_status', 'Pending');
        }]);

        return $user;
    });

    Route::get('/users', function (Request $request) {
        return User::all();
    });

    Route::name('api')->group(function () {
        Route::apiResource(
            'favourites',
            'App\Http\Controllers\Api\FavouriteController'
        );
        Route::apiResource(
            'cart',
            'App\Http\Controllers\Api\CartController'
        );

        Route::apiResource(
            'reviews',
            'App\Http\Controllers\Api\ReviewController'
        )->except(['index', 'create', 'edit']);

        Route::apiResource(
            'sales',
            'App\Http\Controllers\Api\SaleController'
        );
    });
});

Route::get('/tokens/create', function (Request $request) {
    $token = User::first()->createToken('MyToken');

    return ['token' => $token->plainTextToken];
});
Route::post('/login', [App\Http\Controllers\Api\AuthController::class, 'login'])->name('api.auth.login');
Route::post('/register', [App\Http\Controllers\Api\AuthController::class, 'register'])->name('api.auth.register');

Route::get('/search', [App\Http\Controllers\Api\ProductController::class, 'search'])->name('api.products.search');
Route::get('/products/bulk', [App\Http\Controllers\Api\ProductController::class, 'bulk'])->name('api.products.bulk');
Route::get('/filter', [App\Http\Controllers\Api\ProductController::class, 'filter'])->name('api.products.filter');
Route::get('/products/newest', [App\Http\Controllers\Api\ProductController::class, 'newest'])->name('api.products.newest');
Route::get('/products/popular', [App\Http\Controllers\Api\ProductController::class, 'popular'])->name('api.products.popular');
Route::get('/products/gaming', [App\Http\Controllers\Api\ProductController::class, 'gaming'])->name('api.products.gaming');
Route::get('/autocomplete', [App\Http\Controllers\Api\ProductController::class, 'autocomplete'])->name('api.products.autocomplete');
Route::get('/categories/{category}/search', [App\Http\Controllers\Api\CategoryBrandController::class, 'search'])->name('api.categories.search');
Route::get('/categories', [App\Http\Controllers\Api\CategoryBrandController::class, 'categories'])->name('api.categories');
Route::get('/brands', [App\Http\Controllers\Api\CategoryBrandController::class, 'brands'])->name('api.brands');
Route::get('/categories/{category}/filter', [App\Http\Controllers\Api\CategoryBrandController::class, 'filter'])->name('api.categories.filter');
Route::get('/categories/{category}/subcategories', [App\Http\Controllers\Api\CategoryBrandController::class, 'subcategories'])->name('api.categories.subcategories');
Route::name('api.')->group(function () {
    Route::apiResource('products', App\Http\Controllers\Api\ProductController::class);
    Route::apiResource(
        'promotions',
        'App\Http\Controllers\Api\PromotionController'
    )->only(['index', 'show']);
    Route::apiResource(
        'reviews',
        'App\Http\Controllers\Api\ReviewController'
    )->only(['index']);
});
