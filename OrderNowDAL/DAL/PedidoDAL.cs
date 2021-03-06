using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderNowDAL.DAL
{
    public class PedidoDAL
    {
        private OrderNowBDEntities nowBDEntities = new OrderNowBDEntities();

        public Pedido Add(Pedido p)
        {
            Pedido obj = nowBDEntities.Pedido.Add(p);
            nowBDEntities.SaveChanges();
            return obj;
        }
        public void Remove(string id)
        {
            Pedido p = nowBDEntities.Pedido.Find(id);
            nowBDEntities.Pedido.Remove(p);
            nowBDEntities.SaveChanges();
        }
        public void Edit(Pedido p)
        {
            Pedido pedido = nowBDEntities.Pedido.FirstOrDefault(obj => obj.IdPedido == p.IdPedido);
            pedido = p;
            nowBDEntities.SaveChanges();
        }
        public Pedido Find(int id)
        {
            Pedido pedido = nowBDEntities.Pedido.FirstOrDefault(obj => obj.IdPedido == id);
            return pedido;
        }
        public List<Pedido> GetAll()
        {
            return nowBDEntities.Pedido.ToList();
        }
        public List<Pedido> GetAll(int id)
        {
            //Linq
            var query = from c in nowBDEntities.Pedido
                        where c.IdEstadoPedido == id
                        select c;
            return query.ToList();
        }

        public int GetTotal(int id)
        {
            int total = 0;
            List<AlimentoPedido> alimentos = nowBDEntities.AlimentoPedido.Where(x=>x.IdPedido==id).ToList();
            alimentos.ForEach(x =>
            {
                total += nowBDEntities.Alimento.FirstOrDefault(p => p.IdAlimento == x.IdAlimento).Precio.Value;
            });

            return total;
        }
    }
}
