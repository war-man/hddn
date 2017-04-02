﻿using HDNHD.Core.Repositories.Interfaces;
using HoaDonNuocHaDong.Areas.ThuNgan.Models;
using System.Linq;

namespace HoaDonNuocHaDong.Areas.ThuNgan.Repositories.Interfaces
{
    public interface IHoaDonRepository : IRepository<HDNHD.Models.DataContexts.Hoadonnuoc>
    {
        IQueryable<HoaDonModel> GetAllAsHoaDonModel();
        
        IQueryable<DuNoModel> GetAllAsDuNoModel();
    }
}