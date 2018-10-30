using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserTOSPage : ContentPage
    {
        public UserTOSPage()
        {
            InitializeComponent();

            //DependencyService.Get<IAnalytics>().GAScreen("View Terms of Service");
        }

        private void initLocalizedText()
        {
            //topBar.Title = AppResources.UserTOSPage_TermsAndConditions;
            //btnCancel.Text = AppResources.UserTOSPage_CompletedRead;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html =
                @"
<html>
<body>
<style>
	body { font-family: Arial; padding: 20px; }
</style>
<h3>
	1. Terms
</h3>

<p>
	By using this application, you are agreeing to be bound by these 
	application Terms and Conditions of Use, all applicable laws and regulations, 
	and agree that you are responsible for compliance with any applicable local 
	laws. If you do not agree with any of these terms, you are prohibited from 
	using or accessing this application. The materials contained in this application are 
	protected by applicable copyright and trade mark law.
</p>

<h3>
	2. Use License
</h3>

<ol type=""a"">
	<li>
		Permission is granted to temporarily use this application for personal, 
		non-commercial transitory viewing only. This is the grant of a license, 
		not a transfer of title, and under this license you may not:
		
		<ol type=""i"">
			<li>modify or copy the assets (images, text);</li>
			<li>use the materials for any commercial purpose, or for any public display (commercial or non-commercial);</li>
			<li>attempt to decompile or reverse engineer any software contained in MyCompany's application;</li>
			<li>remove any copyright or other proprietary notations from the application.</li>
		</ol>
	</li>
	<li>
		This license shall automatically terminate if you violate any of these restrictions and may be terminated by MyCompany at any time. Upon terminating your viewing of these materials or upon the termination of this license, you must destroy any downloaded materials in your possession whether in electronic or printed format.
	</li>
</ol>

<h3>
	3. Disclaimer
</h3>

<ol type=""a"">
	<li>
		The application is provided ""as is"". MyCompany makes no warranties, expressed or implied, and hereby disclaims and negates all other warranties, including without limitation, implied warranties or conditions of merchantability, fitness for a particular purpose, or non-infringement of intellectual property or other violation of rights. Further, MyCompany does not warrant or make any representations concerning the accuracy, likely results, or reliability of the use of the materials on its application or otherwise relating to such materials or on any applications or sites linked to this application.
	</li>
</ol>

<h3>
	4. Limitations
</h3>

<p>
	In no event shall MyCompany or its suppliers be liable for any damages (including, without limitation, damages for loss of data or profit, or due to business interruption,) arising out of the use or inability to use the application, even if MyCompany or a MyCompany authorized representative has been notified orally or in writing of the possibility of such damage. 
</p>


<h2>
	Privacy Policy
</h2>

<p>
	Your privacy is very important to us. Accordingly, we have developed this Policy in order for you to understand how we collect, use, communicate and disclose and make use of personal information. The following outlines our privacy policy.
</p>

<ul>
	<li>
		Before or at the time of collecting personal information, we will identify the purposes for which information is being collected.
	</li>
	<li>
		We will collect and use of personal information solely with the objective of fulfilling those purposes specified by us and for other compatible purposes, unless we obtain the consent of the individual concerned or as required by law.		
	</li>
	<li>
		We will only retain personal information as long as necessary for the fulfillment of those purposes. 
	</li>
	<li>
		We will collect personal information by lawful and fair means and, where appropriate, with the knowledge or consent of the individual concerned. 
	</li>
	<li>
		Personal data should be relevant to the purposes for which it is to be used, and, to the extent necessary for those purposes, should be accurate, complete, and up-to-date. 
	</li>
	<li>
		We will protect personal information by reasonable security safeguards against loss or theft, as well as unauthorized access, disclosure, copying, use or modification.
	</li>
	<li>
		We will make readily available to customers information about our policies and practices relating to the management of personal information. 
	</li>
</ul>

<p>
	We are committed to conducting our business in accordance with these principles in order to ensure that the confidentiality of personal information is protected and maintained. 
</p>		

			
</body>
<html>
";
            wv.Source = htmlSource;


        }
    }
}

