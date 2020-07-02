package mono.com.google.firebase;


public class FirebaseApp_IdTokenListenersCountChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.firebase.FirebaseApp.IdTokenListenersCountChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onListenerCountChanged:(I)V:GetOnListenerCountChanged_IHandler:Firebase.FirebaseApp/IIdTokenListenersCountChangedListenerInvoker, Xamarin.Firebase.Common\n" +
			"";
		mono.android.Runtime.register ("Firebase.FirebaseApp+IIdTokenListenersCountChangedListenerImplementor, Xamarin.Firebase.Common", FirebaseApp_IdTokenListenersCountChangedListenerImplementor.class, __md_methods);
	}


	public FirebaseApp_IdTokenListenersCountChangedListenerImplementor ()
	{
		super ();
		if (getClass () == FirebaseApp_IdTokenListenersCountChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Firebase.FirebaseApp+IIdTokenListenersCountChangedListenerImplementor, Xamarin.Firebase.Common", "", this, new java.lang.Object[] {  });
	}


	public void onListenerCountChanged (int p0)
	{
		n_onListenerCountChanged (p0);
	}

	private native void n_onListenerCountChanged (int p0);

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
