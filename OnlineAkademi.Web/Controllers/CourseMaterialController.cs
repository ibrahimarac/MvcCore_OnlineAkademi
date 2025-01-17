﻿using AutoMapper;
using OnlineAkademi.Core.Domain.Common;
using OnlineAkademi.Core.Domain.Dto;
using OnlineAkademi.Core.Services;
using OnlineAkademi.Utils.Extensions;
using OnlineAkademi.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineAkademi.Core.Domain.Dto.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace OnlineAkademi.Web.Controllers
{
    [Authorize(Roles = "trainer")]
    public class CourseMaterialController : Controller
    {
        private readonly IMaterialService Materials;
        private readonly IWebHostEnvironment _environment;
        private readonly IAccountService Accounts;
        private readonly IMapper Mapper;

        public CourseMaterialController(IMaterialService materialServices,
            IWebHostEnvironment environment,
            IAccountService accounService, IMapper mapper)
        {
            Materials = materialServices;
            Accounts = accounService;
            Mapper = mapper;
            _environment = environment;
        }

        [HttpGet]
        [Route("CourseMaterial/ListCourses")]
        public IActionResult ListCourses()
        {
            var userName = User.Identity.Name;

            var courseDtos = Materials.GetCourseByTrainer(userName);

            //Dto to VM
            var courseVM = Mapper.Map<IEnumerable<CourseDto>, IEnumerable<CourseVM>>(courseDtos);
            return View(courseVM);
        }


        [HttpGet]
        [Route("CourseMaterial/ListMaterials/{id:int?}")]
        public IActionResult ListMaterials(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("ListCourses").ShowMessage(JConfirmMessageType.Warning,"Dikkat","Kurs listesinden herhangi bir kurs seçmeden devam edemezsiniz.");

            //Şu an üzerinde çalışılan kursun id değerini Session içerisinde saklayalım.
            //Bu bilgi ilerde matryal eklerken gerekli olacak.
            HttpContext.Session.SetInt32("CourseId", id.Value);

            var courseMaterials = Materials.GetCourseMaterials(id);
            var materialsVM = Mapper.Map<IEnumerable<MaterialDto>, IEnumerable<MaterialVM>>(courseMaterials);
            return View(materialsVM);
        }


        [HttpPost]
        [Route("CourseMaterial/RemoveMaterial")]
        public IActionResult RemoveMaterial([Bind("Id")] JQueryDeleteObject model)
        {
            //Fiziksel dosyanın yolu
            var fileName = Materials.GetFileUrl(model.Id);
            var fileUrl = Path.Combine(_environment.WebRootPath, "materials", fileName);

            //Veritabanından sil
            Materials.DeleteMaterial(model.Id);

            //Fiziksel dosya varsa kaldır
            if (string.IsNullOrEmpty(fileUrl))
            {
                if (System.IO.File.Exists(fileUrl))
                    System.IO.File.Delete(fileUrl);
            }

            return Json(new JsonResponse
            {
                Status = JsonResponseStatus.Ok,
                Message = "Materyal başarıyla silindi."
            });
        }

        [HttpGet]
        [Route("CourseMaterial/AddMaterial")]
        public IActionResult AddMaterial()
        {
            //materyal eklenecek kurs numarası
            var courseId = HttpContext.Session.GetInt32("CourseId");
            return View();
        }

        [HttpPost]
        [Route("CourseMaterial/AddMaterial")]
        public IActionResult AddMaterial(MaterialVM material)
        {
            //materyal eklenecek kurs numarası
            var courseId = HttpContext.Session.GetInt32("CourseId");

            //Dosya Seçilmişse dosyayı yükleyelim
            if (material.UploadedFile != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(material.UploadedFile.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "materials", fileName);
                if (material.UploadedFile != null)
                {
                    material.UploadedFile.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                material.Url = fileName;
            }
            material.CourseId = courseId.Value;
            
            var materialDto = Mapper.Map<MaterialVM, MaterialDto>(material);

            Materials.AddMaterial(materialDto);

            return RedirectToAction("ListMaterials", new { id = courseId })
                .ShowMessage(JConfirmMessageType.Success, "Uyarı", "Materyal başarıyla kaydedildi.");
        }

    }
}
