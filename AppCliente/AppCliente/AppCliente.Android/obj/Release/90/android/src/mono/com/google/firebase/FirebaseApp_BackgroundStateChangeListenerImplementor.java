package mono.com.google.firebase;


public class FirebaseApp_BackgroundStateChangeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.firebase.FirebaseApp.BackgroundStateChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBackgroundStateChanged:(Z)V:GetOnBackgroundStateChanged_ZHandler:Firebase.FirebaseApp/IBackgroundStateChangeListenerInvoker, Xamarin.Firebase.Common\n" +
			"";
		mono.android.Runtime.register ("Firebase.FirebaseApp+IBackgroundStateChangeListenerImplementor, Xamarin.Firebase.Common", FirebaseApp_BackgroundStateChangeListenerImplementor.class, __md_methods);
	}


	public FirebaseApp_BackgroundStateChangeListenerImplementor ()
	{
		super ();
		if (getClass () == FirebaseApp_BackgroundStateChangeListenerImplementor.class)
			mono.android.TypeManager.Activate ("Firebase.FirebaseApp+IBackgroundStateChangeListenerImplementor, Xamarin.Firebase.Common", "", this, new java.lang.Object[] {  });
	}


	public void onBackgroundStateChanged (boolean p0)
	{
		n_onBackgroundStateChanged (p0);
	}

	private native void n_onBackgroundStateChanged (boolean p0);

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
