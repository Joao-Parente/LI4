package crc6421f751c7745e34df;


public class OSExternalUserIdUpdateCompletionHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.onesignal.OneSignal.OSExternalUserIdUpdateCompletionHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onComplete:(Lorg/json/JSONObject;)V:GetOnComplete_Lorg_json_JSONObject_Handler:Com.OneSignal.Android.OneSignal/IOSExternalUserIdUpdateCompletionHandlerInvoker, OneSignal.Android.Binding\n" +
			"";
		mono.android.Runtime.register ("Com.OneSignal.OSExternalUserIdUpdateCompletionHandler, Com.OneSignal", OSExternalUserIdUpdateCompletionHandler.class, __md_methods);
	}


	public OSExternalUserIdUpdateCompletionHandler ()
	{
		super ();
		if (getClass () == OSExternalUserIdUpdateCompletionHandler.class)
			mono.android.TypeManager.Activate ("Com.OneSignal.OSExternalUserIdUpdateCompletionHandler, Com.OneSignal", "", this, new java.lang.Object[] {  });
	}


	public void onComplete (org.json.JSONObject p0)
	{
		n_onComplete (p0);
	}

	private native void n_onComplete (org.json.JSONObject p0);

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
