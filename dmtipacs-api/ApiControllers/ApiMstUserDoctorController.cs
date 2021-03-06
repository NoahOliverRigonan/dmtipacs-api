﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace dmtipacs_api.ApiControllers
{
    [Authorize, RoutePrefix("api/userDoctor")]
    public class ApiMstUserDoctorController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.dmtipacsdbDataContext db = new Data.dmtipacsdbDataContext();

        // ==================
        // List - User Doctor
        // ==================
        [HttpGet, Route("list/{userId}")]
        public List<Entities.MstUserDoctor> ListUserDoctor(String userId)
        {
            var userDoctors = from d in db.MstUserDoctors.OrderByDescending(d => d.Id)
                              where d.UserId == Convert.ToInt32(userId)
                              select new Entities.MstUserDoctor
                              {
                                  Id = d.Id,
                                  UserId = d.UserId,
                                  DoctorId = d.DoctorId,
                                  Doctor = d.MstUser1.FullName
                              };

            return userDoctors.ToList();
        }

        // =================
        // Add - User Doctor
        // =================
        [HttpPost, Route("add")]
        public HttpResponseMessage AddUserDoctor(Entities.MstUserDoctor objUserDoctor)
        {
            try
            {
                Data.MstUserDoctor newUserDoctor = new Data.MstUserDoctor
                {
                    UserId = objUserDoctor.UserId,
                    DoctorId = objUserDoctor.DoctorId,
                };

                db.MstUserDoctors.InsertOnSubmit(newUserDoctor);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // ====================
        // Update - User Doctor
        // ====================
        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateUserDoctor(String id, Entities.MstUserDoctor objUserDoctor)
        {
            try
            {
                var userDoctor = from d in db.MstUserDoctors
                                 where d.Id == Convert.ToInt32(id)
                                 select d;

                if (userDoctor.Any())
                {
                    var updateUserDoctor = userDoctor.FirstOrDefault();
                    updateUserDoctor.UserId = objUserDoctor.UserId;
                    updateUserDoctor.DoctorId = objUserDoctor.DoctorId;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // ====================
        // Delete - User Doctor
        // ====================
        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteUserDoctor(String id)
        {
            try
            {
                var userDoctor = from d in db.MstUserDoctors
                                 where d.Id == Convert.ToInt32(id)
                                 select d;

                if (userDoctor.Any())
                {
                    db.MstUserDoctors.DeleteOnSubmit(userDoctor.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}