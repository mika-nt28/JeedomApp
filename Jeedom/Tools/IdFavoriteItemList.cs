using Jeedom.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jeedom.Tools
{
    public class IdFavoriteItemList : ObservableCollection<EqLogic>
    {
        private List<string> _idList = new List<string>();

        public IdFavoriteItemList()
        {
        }

        public List<string> IdList
        {
            get
            {
                return _idList;
            }
            set
            {
                _idList = value;
            }
        }

        public new void Add(EqLogic eq)
        {
            if (!Contains(eq))
            {
                IdList.Add(eq.Id);
                base.Add(eq);
            }
        }

        public new void Clear()
        {
            IdList.Clear();
            base.Clear();
        }

        public void PopulateFromEqLogicAndSceneList(ObservableCollection<EqLogic> eqLogicList, ObservableCollection<Scene> sceneList)
        {
            //Todo: stocker le type des item dans la liste
            base.Clear();
            foreach (string id in _idList)
            {
                var lst = from e in eqLogicList where e.Id == id select e;
                if (lst.Count() != 0)
                {
                    var e = lst.First();
                    base.Add(e);
                }
                else
                    _idList.Remove(id);
            }
        }

        public new bool Remove(EqLogic eq)
        {
            IdList.Remove(eq.Id);
            return base.Remove(eq);
        }
    }
}