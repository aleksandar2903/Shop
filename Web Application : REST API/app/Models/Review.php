<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Support\Facades\DB;

class Review extends Model
{
    use HasFactory, SoftDeletes;

    protected $fillable = [
        'user_id', 'product_id', 'rating', 'text', 'title', 'published', 'admin_id'
    ];

    protected $appends = ['author'];

    public function user()
    {
        return $this->belongsTo('App\Models\User', 'user_id');
    }

    public function product()
    {
        return $this->belongsTo('App\Models\Product', 'product_id');
    }

    public function admin()
    {
        return $this->belongsTo('App\Models\User', 'admin_id');
    }

    public function getAuthorAttribute()
    {
        return $this->user->name;
    }
}
