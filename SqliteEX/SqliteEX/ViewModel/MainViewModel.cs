using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Data.SQLite;

namespace SqliteEX.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            MyClick = new RelayCommand<string>(CreateDB);           
        }
        public RelayCommand<string> MyClick { get; set; }
        /// <summary>
        /// 创建一个空的数据库
        /// </summary>
        /// <param name="str"></param>
        private void CreateDB(string str)
        {
            SQLiteConnection.CreateFile(str);
        }
    }
}