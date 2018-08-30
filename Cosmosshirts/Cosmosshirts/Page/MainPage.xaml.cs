using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmosshirts.PageModel;
using Cosmosshirts.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cosmosshirts.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage
	{
	    private IDatabaseService _databaseService;
		public MainPage (IDatabaseService databaseService)
		{
			InitializeComponent ();
		    _databaseService = databaseService;

	        BindingContext = new MainPageModel(_databaseService);
		}

	    public void DeselectItem()
	    {
	        MenuListView.SelectedItem = null;
	    }

	}
}