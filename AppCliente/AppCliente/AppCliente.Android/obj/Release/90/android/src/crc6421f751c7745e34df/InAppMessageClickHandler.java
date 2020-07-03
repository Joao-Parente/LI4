package crc6421f751c7745e34df;


public class InAppMessageClickHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.onesignal.OneSignal.InAppMessageClickHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_inAppMessageClicked:(Lcom/onesignal/OSInAppMessageAction;)V:GetInAppMessageClicked_Lcom_onesignal_OSInAppMessageAction_Handler:Com.OneSignal.Android.OneSignal/IInAppMessageClickHandlerInvoker, OneSignal.Android.Binding\n" +
			"";
		mono.android.Runtime.register ("Com.OneSignal.InAppMessageClickHandler, Com.OneSignal", InAppMessageClickHandler.class, __md_methods);
	}


	public InAppMessageClickHandler ()
	{
		super ();
		if (getClass () == InAppMessageClickHandler.class)
			mono.android.TypeManager.Activate ("Com.OneSignal.InAppMessageClickHandler, Com.OneSignal", "", this, new java.lang.Object[] {  });
	}


	public void inAppMessageClicked (com.onesignal.OSInAppMessageAction p0)
	{
		n_inAppMessageClicked (p0);
	}

	private native void n_inAppMessageClicked (com.onesignal.OSInAppMessageAction p0);

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
