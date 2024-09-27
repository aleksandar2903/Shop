<?php

namespace App\Http\Controllers;

use App\Models\Product;
use App\Models\ProductCategory;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\File;
use Illuminate\Support\Facades\Validator;
use Illuminate\Support\MessageBag;
use Illuminate\Validation\Rule;
use Intervention\Image\Facades\Image;


class ProductCategoryController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(ProductCategory $model)
    {
        $categories = ProductCategory::latest('updated_at')->get();

        return view('products.categories.index', compact('categories'));
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        //
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request, ProductCategory $category)
    {
        $request->validate([
            'name' => 'required|min:3|unique:product_categories,name',
            'image' => 'required|image|mimes:jpeg,png,jpg,gif,svg|max:2048'
        ]);

        $data = $request->all();

        $img = Image::make($request->image->path());
        $imageName = time() . $request->image->extension();
        $img->resize(500, 500, function ($constraint) {
            $constraint->aspectRatio();
        })->save(public_path('/storage/images/' . $imageName));
        $data['image'] = $imageName;


        $category->create($data);
        return redirect('/products/categories')
            ->withStatus(__('Category successfully created.'));
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show(ProductCategory $category)
    {
        return view('products.categories.show', [
            'category' => $category,
            'products' => Product::where(function ($query) use ($category) {
                $query->where('product_category_id', $category->id);
            })->latest()->get()
        ]);
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit(ProductCategory $category)
    {
        return view('products.categories.edit', compact('category'));
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, ProductCategory $category)
    {
        $request->validate([
            'name' => 'required|min:3',
            'image' => 'image|mimes:jpeg,png,jpg,gif,svg|max:2048'
        ]);

        $data = $request->all();
        if ($request->image) {
            File::delete(public_path('/storage/images/' . $category->image));
            $img = Image::make($request->image->path());
            $imageName = time() . $request->image->extension();
            $img->resize(500, 500, function ($constraint) {
                $constraint->aspectRatio();
            })->save(public_path('/storage/images/' . $imageName));
            $data['image'] = $imageName;
        }

        $category->update($data);

        return redirect('/products/categories')
            ->withStatus(__('Category successfully updated.'));
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(ProductCategory $category)
    {
        if ($category->image) {
            File::delete(public_path('/storage/images/' . $category->image));
        }
        $category->delete();

        return redirect('/products/categories')
            ->withStatus(__('Category successfully deleted.'));
    }
}
