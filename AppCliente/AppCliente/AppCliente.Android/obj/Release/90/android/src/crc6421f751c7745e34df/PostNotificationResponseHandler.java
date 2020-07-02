package crc6421f751c7745e34df;


public class PostNotificationResponseHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.onesignal.OneSignal.PostNotificationResponseHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFailure:(Lorg/json/JSONObject;)V:GetOnFailure_Lorg_json_JSONObject_Handler:Com.OneSignal.Android.OneSignal/IPostNotificationResponseHandlerInvoker, OneSignal.Android.Binding\n" +
			"n_onSuccess:(Lorg/json/JSONObject;)V:GetOnSuccess_Lorg_json_JSONObject_Handler:Com.OneSignal.Android.OneSignal/IPostNotificationResponseHandlerInvoker, OneSignal.Android.Binding\n" +
			"";
		mono.android.Runtime.register ("Com.OneSignal.PostNotificationResponseHandler, Com.OneSignal", PostNotificationResponseHandler.class, __md_methods);
	}


	public PostNotificationResponseHandler ()
	{
		super ();
		if (getClass () == PostNotificationResponseHandler.class)
			mono.android.TypeManager.Activate ("Com.OneSignal.PostNotificationResponseHandler, Com.OneSignal", "", this, new java.lang.Object[] {  });
	}


	public void onFailure (org.json.JSONObject p0)
	{
		n_onFailure (p0);
	}

	private native void n_onFailure (org.json.JSONObject p0);


	public void onSuccess (org.json.JSONObject p0)
	{
		n_onSuccess (p0);
	}

	private native void n_onSuccess (org.json.JSONObject p0);

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
