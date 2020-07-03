package crc6421f751c7745e34df;


public class NotificationReceivedHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.onesignal.OneSignal.NotificationReceivedHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_notificationReceived:(Lcom/onesignal/OSNotification;)V:GetNotificationReceived_Lcom_onesignal_OSNotification_Handler:Com.OneSignal.Android.OneSignal/INotificationReceivedHandlerInvoker, OneSignal.Android.Binding\n" +
			"";
		mono.android.Runtime.register ("Com.OneSignal.NotificationReceivedHandler, Com.OneSignal", NotificationReceivedHandler.class, __md_methods);
	}


	public NotificationReceivedHandler ()
	{
		super ();
		if (getClass () == NotificationReceivedHandler.class)
			mono.android.TypeManager.Activate ("Com.OneSignal.NotificationReceivedHandler, Com.OneSignal", "", this, new java.lang.Object[] {  });
	}


	public void notificationReceived (com.onesignal.OSNotification p0)
	{
		n_notificationReceived (p0);
	}

	private native void n_notificationReceived (com.onesignal.OSNotification p0);

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
