package crc6421f751c7745e34df;


public class SendOutcomeEventSuccessHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.onesignal.OneSignal.OutcomeCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSuccess:(Lcom/onesignal/OutcomeEvent;)V:GetOnSuccess_Lcom_onesignal_OutcomeEvent_Handler:Com.OneSignal.Android.OneSignal/IOutcomeCallbackInvoker, OneSignal.Android.Binding\n" +
			"";
		mono.android.Runtime.register ("Com.OneSignal.SendOutcomeEventSuccessHandler, Com.OneSignal", SendOutcomeEventSuccessHandler.class, __md_methods);
	}


	public SendOutcomeEventSuccessHandler ()
	{
		super ();
		if (getClass () == SendOutcomeEventSuccessHandler.class)
			mono.android.TypeManager.Activate ("Com.OneSignal.SendOutcomeEventSuccessHandler, Com.OneSignal", "", this, new java.lang.Object[] {  });
	}


	public void onSuccess (com.onesignal.OutcomeEvent p0)
	{
		n_onSuccess (p0);
	}

	private native void n_onSuccess (com.onesignal.OutcomeEvent p0);

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
