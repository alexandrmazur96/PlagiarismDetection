using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAdder.Types;

namespace DocumentAdder.Model
{
    public class MainSettingModel : BaseModel
    {
        #region Fields        
        private List<Pages> _settingsPages;
        #endregion

        #region Properties

        /// <summary>
        /// Возвращает коллекцию страниц, которые отображаются в табе "Настройки"
        /// </summary>
        public List<Pages> SettingsPages
        {
            get
            {
                return _settingsPages;
            }
        }

        #endregion

        /// <summary>
        /// Создает модель настроек, 
        /// инициализирует коллекцию путей к директориям, где лежат файлы,
        /// задает возможные форматы файлов для чтения.
        /// </summary>
        public MainSettingModel()
        {
            _settingsPages = new List<Pages>();
            PagesInit();
        }

        /// <summary>
        /// Инициализирует страницы. Используется внутри конструктора.
        /// </summary>        
        private void PagesInit()
        {
            _settingsPages.Add(new Pages("Приложение", new View.SettingsViews.ServerWorkingSettingsView()));
            _settingsPages.Add(new Pages("База данных", new View.SettingsViews.DatabaseSettingsView()));
            _settingsPages.Add(new Pages("Репозиторий", new View.SettingsViews.RepositorySettingsView()));
            _settingsPages.Add(new Pages("Другие", new View.SettingsViews.OtherSettingsView()));
        }
    }
}
