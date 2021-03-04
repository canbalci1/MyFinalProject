﻿using Business.Abstract;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [ValidationAspect(typeof(ProductValidator))]

        public IResult Add(Product product)
        {
            

            ValidationTool.Validate(new ProductValidator(), product);
           
            //business codes
            
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAddded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 06)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            //iş kodları
            //Yetkisi var mı ?
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);
        }

        public IDataResult< List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>( _productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult <Product> GetById(int productId)
        {
            return new SuccessDataResult<Product> (_productDal.Get(p=> p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult< List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>( _productDal.GetProductDetails());
        }
    }
}
