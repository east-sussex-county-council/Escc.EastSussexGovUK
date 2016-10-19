<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="1Space.ascx.cs" Inherits="Escc.EastSussexGovUK.WebForms.EastSussex1Space" %>
<%@ Register TagPrefix="ClientDependency" Namespace="Escc.ClientDependencyFramework.WebForms" assembly="Escc.ClientDependencyFramework.WebForms" %>
<%@ Register TagPrefix="EastSussexGovUK" Namespace="Escc.EastSussexGovUK.WebForms" Assembly="Escc.EastSussexGovUK.WebForms" %>
<EastSussexGovUK:ContextContainer runat="server" Plain="false">
<ClientDependency:Css runat="server" Files="EastSussex1Space" />
<ClientDependency:Script runat="server" Files="EastSussex1Space" />
<div class="supporting large">
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
</EastSussexGovUK:ContextContainer>