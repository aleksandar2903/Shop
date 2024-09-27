<?php

namespace App\Models;

use Illuminate\Contracts\Auth\MustVerifyEmail;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Notifications\Notifiable;
use Laravel\Sanctum\HasApiTokens;

class User extends Authenticatable
{
    use HasFactory, Notifiable, HasApiTokens;

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = [
        'name',
        'email',
        'password',
    ];

    /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'password',
        'remember_token',
    ];

    /**
     * The attributes that should be cast to native types.
     *
     * @var array
     */
    protected $casts = [
        'email_verified_at' => 'datetime',
    ];

    public function carts()
    {
        return $this->hasMany('App\Models\Cart');
    }

    public function favourites()
    {
        return $this->hasMany('App\Models\Favourite');
    }

    public function reviews()
    {
        return $this->hasMany('App\Models\Review', 'user_id');
    }

    public function client()
    {
        return $this->hasOne('App\Models\Client', 'document_id');
    }

    public function client_orders()
    {
        return $this->hasManyThrough('App\Models\Sale', 'App\Models\Client', 'email', 'client_id', 'email');
    }
}
