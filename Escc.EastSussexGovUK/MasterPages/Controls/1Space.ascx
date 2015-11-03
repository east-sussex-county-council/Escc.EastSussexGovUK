<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<ClientDependency:Css runat="server" Files="1Space" />
<ClientDependency:Script runat="server" Files="1Space" />
<div class="supporting">
    <div id="marketplace-search-widget">
	<div id="logo">
		<img src="https://www.eastsussex1space.co.uk/skins/eastsussex/Images/widgetLogo.png" alt="East Sussex 1Space logo" />
	</div>
	<p id="text-area-1">
		Find the help you need
	</p>
	<div id="bottom-region">
		<p id="text-area-2">
			A directory of services, groups and organisations
		</p>
		<form method="GET" action="https://www.eastsussex1space.co.uk/Search">
			<input type="text" name="keywords" value="What are you looking for?" />
			<button type="submit" name="Go" class="button">
				<div>
					<span class="search">Go</span>
				</div>
			</button>
		</form>
	</div>
</div>
</div>