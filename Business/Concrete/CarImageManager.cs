﻿using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.FileHelper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;
        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        public IResult Add(CarImage carImage, IFormFile file)
        {
            var result = BusinessRules.Run(CheckIfImageExtensionValid(file), CheckIfImageLimitExpired(carImage.CarId));
            if (result != null)
            {
                return result;
            }


            carImage.ImagePath = CarImagesFileHelper.Add(file);
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.CarImageAdded);
        }

        public IResult Delete(CarImage carImage)
        {
            var result = BusinessRules.Run(CheckIfImageExists(carImage.Id));
            if (result != null)
            {
                return result;
            }
            string path = GetById(carImage.Id).Data.ImagePath;
            CarImagesFileHelper.Delete(path);
            _carImageDal.Delete(carImage);
            return new SuccessResult(Messages.CarImageDeleted);

        }

        public IResult DeleteByCarId(int carId)
        {
            var result = _carImageDal.GetAll(c => c.CarId == carId);
            if (result.Any())
            {
                foreach (var carImage in result)
                {
                    Delete(carImage);
                }
                return new SuccessResult();
            }
            return new ErrorResult(Messages.CarHaveNoImage);
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll());
        }

        public IDataResult<List<CarImage>> GetAllByCarId(int carId)
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(c => c.CarId == carId));
        }

        public IDataResult<CarImage> GetById(int id)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.Id == id));
        }

        public IResult Update(CarImage carImage, IFormFile file)
        {
            IResult result = BusinessRules.Run(
                  CheckIfImageLimitExpired(carImage.CarId),
                  CheckIfImageExtensionValid(file),
                  CheckIfImageExists(carImage.Id)
                  );

            if (result != null)
            {
                return result;
            }
            CarImage oldCarImage = GetById(carImage.Id).Data;
            carImage.ImagePath = CarImagesFileHelper.Update(file, oldCarImage.ImagePath);
            carImage.Date = DateTime.Now;
            carImage.CarId = oldCarImage.CarId;
            _carImageDal.Update(carImage);
            return new SuccessResult();
        }

        private IResult CheckIfImageLimitExpired(int carId)
        {
            int result = _carImageDal.GetAll(c => c.CarId == carId).Count;
            if (result > 5)
            {
                return new ErrorResult(Messages.ImageLimitExpiredForCar);
            }
            return new SuccessResult();
        }

        private IResult CheckIfImageExtensionValid(IFormFile file)
        {
            bool isValidFileExtension = Messages.ValidImageFileTypes.Any(t => t == Path.GetExtension(file.FileName).ToUpper());
            if (!isValidFileExtension)
                return new ErrorResult(Messages.InvalidImageExtension);
            return new SuccessResult();
        }

        private IResult CheckIfImageExists(int id)
        {
            if (_carImageDal.IsExists(id))
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.CarImageMustbeExists);
        }

        public IDataResult<List<CarImageDto>> GetCarImageDetails(int id)
        {
            return new SuccessDataResult<List<CarImageDto>>(_carImageDal.GetCarImagesDetail(p => p.CarId == id), Messages.Listed);
        }
    }

}
