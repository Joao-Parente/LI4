package mono.com.google.firebase.analytics.connector;


public class AnalyticsConnector_AnalyticsConnectorListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.firebase.analytics.connector.AnalyticsConnector.AnalyticsConnectorListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMessageTriggered:(ILandroid/os/Bundle;)V:GetOnMessageTriggered_ILandroid_os_Bundle_Handler:Firebase.Analytics.Connector.IAnalyticsConnectorAnalyticsConnectorListenerInvoker, Xamarin.Firebase.Measurement.Connector\n" +
			"";
		mono.android.Runtime.register ("Firebase.Analytics.Connector.IAnalyticsConnectorAnalyticsConnectorListenerImplementor, Xamarin.Firebase.Measurement.Connector", AnalyticsConnector_AnalyticsConnectorListenerImplementor.class, __md_methods);
	}


	public AnalyticsConnector_AnalyticsConnectorListenerImplementor ()
	{
		super ();
		if (getClass () == AnalyticsConnector_AnalyticsConnectorListenerImplementor.class)
			mono.android.TypeManager.Activate ("Firebase.Analytics.Connector.IAnalyticsConnectorAnalyticsConnectorListenerImplementor, Xamarin.Firebase.Measurement.Connector", "", this, new java.lang.Object[] {  });
	}


	public void onMessageTriggered (int p0, android.os.Bundle p1)
	{
		n_onMessageTriggered (p0, p1);
	}

	private native void n_onMessageTriggered (int p0, android.os.Bundle p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
