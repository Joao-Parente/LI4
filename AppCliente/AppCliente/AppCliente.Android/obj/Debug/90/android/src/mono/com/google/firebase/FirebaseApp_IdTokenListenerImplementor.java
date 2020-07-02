package mono.com.google.firebase;


public class FirebaseApp_IdTokenListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.firebase.FirebaseApp.IdTokenListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onIdTokenChanged:(Lcom/google/firebase/internal/InternalTokenResult;)V:GetOnIdTokenChanged_Lcom_google_firebase_internal_InternalTokenResult_Handler:Firebase.FirebaseApp/IIdTokenListenerInvoker, Xamarin.Firebase.Common\n" +
			"";
		mono.android.Runtime.register ("Firebase.FirebaseApp+IIdTokenListenerImplementor, Xamarin.Firebase.Common", FirebaseApp_IdTokenListenerImplementor.class, __md_methods);
	}


	public FirebaseApp_IdTokenListenerImplementor ()
	{
		super ();
		if (getClass () == FirebaseApp_IdTokenListenerImplementor.class)
			mono.android.TypeManager.Activate ("Firebase.FirebaseApp+IIdTokenListenerImplementor, Xamarin.Firebase.Common", "", this, new java.lang.Object[] {  });
	}


	public void onIdTokenChanged (com.google.firebase.internal.InternalTokenResult p0)
	{
		n_onIdTokenChanged (p0);
	}

	private native void n_onIdTokenChanged (com.google.firebase.internal.InternalTokenResult p0);

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
