﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderNowDAL.DAL
{
    public class TipoMedicionDAL
    {
        private OrderNowBDEntities nowBDEntities = new OrderNowBDEntities();

        public void Add(TipoMedicion p)
        {
            nowBDEntities.TipoMedicion.Add(p);
            nowBDEntities.SaveChanges();
        }
        public void Remove(string id)
        {
            TipoMedicion p = nowBDEntities.TipoMedicion.Find(id);
            nowBDEntities.TipoMedicion.Remove(p);
            nowBDEntities.SaveChanges();
        }
        public void Edit(TipoMedicion p)
        {
            TipoMedicion TipoMedicion = nowBDEntities.TipoMedicion.FirstOrDefault(obj => obj.IdTipoMedicion == p.IdTipoMedicion);
            TipoMedicion = p;
            nowBDEntities.SaveChanges();
        }
        public List<TipoMedicion> GetAll()
        {
            return nowBDEntities.TipoMedicion.ToList();
        }
        public TipoMedicion Find(int id)
        {
            TipoMedicion m = nowBDEntities.TipoMedicion.FirstOrDefault(obj => obj.IdTipoMedicion == id);
            return m;
        }
    }
}