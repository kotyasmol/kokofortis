   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5	      3    -TFortisDeviceManager.Views.AuthenticationView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      :local(clr-namespace:TFortisDeviceManager.Views     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      >
properties-clr-namespace:TFortisDeviceManager.Properties  5	      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5
      $	��250��5      $	��350��5        � 
ResizeMode$  NoResize=�5       � Title 5      ��  �� Path$ Title�.��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      �� 5      �� 5      $��50 �6   .+�5   
   �� $��0��6   /   )TFortisDeviceManager.Properties.Resources   EnterLoginAndPassword#��Z 6!   $��Center=�6^   $��Center=�6{   $��14(�6�   $��DemiBold"�6�   �� 5   
   $��1��6   $��Center=�6"   $��Center=�6?    �� Orientation$ 
Horizontal=�6Z   .+�5      ��    	UserLogin#��Z 6    �� TextAlignment$
 Left=�6H   $��60��6]   $
��Wrap=�6h   $��Center=�6|   #  HandyControl.Controls.TextBox  5       �� TextWrapping$
 Wrap=�6=   $	��160��6Q   	 � Cursor$	 IBeam��6]   ��6   �� *Login+
 �� Mode$
 TwoWay=�6]   5      �� 5   
   $��2��6   $��Center=�6"   $��Center=�6?   $ 
Horizontal=�6Z   .+�5      ��    UserPassword#��Z 6   $
 Left=�6L   $��60��6a   $
��Wrap=�6l   $��Center=�6�   '  !HandyControl.Controls.PasswordBox  5       $	��160��6A      ShowEyeButton$
 True��6M      IsSafeEnabled$ False��6b   $	 IBeam��6x      UnsafePassword 6   �� *
Password+6x   5"      �� 5$   
   $��3��5%         Apply#��Z 5&      $��Right=�6B   $��0,5,60,0q�6^   $	��Top=�6p      ButtonPrimary#��[  5'       �� IsCancel$
 True��5(      $
	 Hand��5)      5+      5-      5.      