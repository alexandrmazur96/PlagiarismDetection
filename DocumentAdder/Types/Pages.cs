using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAdder.Types
{
    public class Pages
    {
        #region Fields        
        private string _pageName;
        private System.Windows.Controls.Page _pageRef;
        #endregion

        #region Properties
        /// <summary>
        /// Имя (идентификатор) открываемой страницы 
        /// </summary>
        /// <value>PageName свойство позволяет записать/прочитать значение строкового поля _pageName</value>

        public string PageName
        {
            get
            {
                return _pageName;
            }
            set
            {
                _pageName = value;
            }
        }

        /// <summary>
        /// Ссылка на открываемую страницу.
        /// </summary>
        /// <value>PageRef свойство позволяет записать/прочитать значение строкового поля _pageRef</value>        
        public System.Windows.Controls.Page PageRef
        {
            get
            {
                return _pageRef;
            }
            set
            {
                _pageRef = value;
            }
        }
        #endregion

        /// <summary>
        /// Создает объект страницы на основании
        /// </summary>
        /// <param name="pageName">Имя страницы</param>
        /// <param name="pageRef">Ссылка на объект страницы типа System.Windows.Controls.Page</param>
        public Pages(string pageName, System.Windows.Controls.Page pageRef)
        {
            PageName = pageName;
            PageRef = pageRef;
        }
    }
}
