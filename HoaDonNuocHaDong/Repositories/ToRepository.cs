﻿using HDNHD.Core.Repositories;
using HoaDonNuocHaDong.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace HoaDonNuocHaDong.Repositories
{
    public class ToRepository : LinqRepository<HDNHD.Models.DataContexts.ToQuanHuyen>, IToRepository
    {
        public ToRepository(DataContext context) : base(context) { }

        public IQueryable<HDNHD.Models.DataContexts.ToQuanHuyen> GetByQuanHuyenID(int quanHuyenID)
        {
            return GetAll(m => m.QuanHuyenID == quanHuyenID && m.IsDelete == false);
        }
    }
}