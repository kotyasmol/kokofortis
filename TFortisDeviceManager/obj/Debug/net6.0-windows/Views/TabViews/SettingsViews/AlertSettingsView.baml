   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5      I)clr-namespace:TFortisDeviceManager.ModelsTFortisDeviceManager.Models  6O   2    ,TFortisDeviceManager.Views.AlertSettingsView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      >
properties-clr-namespace:TFortisDeviceManager.Properties  5      6models)clr-namespace:TFortisDeviceManager.Models  6O   .��5      �   � RowDefinitions  5   
   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   .+�5      �� $
��Left=�6   .+�5      �� /   )TFortisDeviceManager.Properties.Resources   AlertSendingSettings#��Z 6   $��	10,30,0,0q�6X   $
��Bold"�6k   $��14(�6}   �� 5      ���	#FFD0D1D7$��	10,0,10,0q�61   $��0,0,0,1q�6D    J� SnapsToDevicePixels$
 True��6^   ._�5      ��  �� Orientation$ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   $��Stretch=�6i   .+�5      �� $��Center=�6%   $	��180��6@   .+�5      ��    SenderAddress#��Z 6(   $
��Wrap=�6]    �� TextAlignment$
 Left=�6q   5       �� 5!      $��Center=�6%   .+�5"      #  HandyControl.Controls.TextBox  $	��250��6)    �� TextWrapping$
 Wrap=�66   $��Right=�6r   $��Center=�6�   ��6J   �� *	FromEmail+ �� Mode$ TwoWay=�6�   5#      5$      5%      �� 5'      ���	#FFD0D1D7$��	10,0,10,0q�61   $��0,0,0,1q�6D   $
 True��6^   ._�5(      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   $��Stretch=�6i   .+�5)      �� $��Center=�6%   $	��180��6@   .+�5*      �� 	   
AlertTitle#��Z	 6(   $
��Wrap=�6Z   $
 Left=�6n   $
��Left=�6�   $��Center=�6�   5+      �� 5,      $��Center=�6%   .+�5-        $	��250��6)   $
 Wrap=�65   ��6I   �� *SubjectForEmail+$ TwoWay=�5.      5/      50      �� 52      ���	#FFD0D1D7$��	10,0,10,0q�61   $��0,0,0,1q�6D   $
 True��6^   ._�53      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   .+�54      �� $��Center=�6%   $	��180��6@   .+�55      �� 
   ReceiversList#��Z
 6(   $
��Wrap=�6]   $
 Left=�6q   $
��Left=�6�   $��Center=�6�   56      �� 57      $��Center=�6%   .+�58        $
 Wrap=�6)   $	��250��6=   ��6I   �� *
ToEmails+$ TwoWay=�59      5;      5<      �� 5>      ���	#FFD0D1D7$��	10,0,10,0q�61   $
 True��6E   ._�5?      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   .+�5@      �� $ Vertical=�6%   $��Center=�6<   $��Center=�6W   $��
10,10,0,10q�6t   .+�5A      �� $ 
Horizontal=�6)   $��10q�6B   .+�5B   "   �� $��Center=�6-   $��60��6H   .+�5C   &   ��    	UserLogin#��Z 60   $��Center=�6a   $
 Left=�6|   $��0,0,0,0q�6�   5D   #     5E   "   $
 Wrap=�6-   $	��150��6e   ��6A   �� *Login+$ TwoWay=�6e   5F      �� 5G      $ 
Horizontal=�6)   $��10q�6B   .+�5H   "   �� $��Center=�6-   $��60��6H   .+�5I   &   �� $
��Wrap=�60      UserPassword#��Z 6D   $��Center=�6x   $
 Left=�6�   $��0,0,0,0q�6�   5J   #   '  !HandyControl.Controls.PasswordBox  5K   "      ShowEyeButton$
 True��6j      IsSafeEnabled$ False��6   $	��150��6�      UnsafePassword 61   �� *PasswordForEmail+$ TwoWay=�6�   5L      5M      �� 5O      $ Vertical=�6%   $��Center=�6<   $��Center=�6W   .+�5P      �� $��Center=�6)   $ 
Horizontal=�6D   $��10q�6]   .+�5Q   "   ��    
SMTPServer#��Z 6,   $ Center=�6^   $��80��6u   $��Center=�6�   $��0,0,20,0q�6�     5S   "   $
 Wrap=�6-   $	��120��6A   ��6M   �� *
SmtpServer+$ TwoWay=�5T      �� 5V      $��Center=�6)   $ 
Horizontal=�6D   $��10q�6]   .+�5W   "   ��    Port#��Z 6,   $ Center=�6X   $��80��6o   $��Center=�6z   $��0,0,20,0q�6�     5Y   "   $
 Wrap=�6-   $	��120��6A   ��6M   �� *	EmailPort+$ TwoWay=�5Z      5\      5]      5^      �� 5`         SendTestNotification#��Z 6   $��	10,30,0,0q�6X   $
��Bold"�6k   $��14(�6}   �� 5b      ���	#FFD0D1D7$��	10,0,10,0q�61   $
 True��6D   ._�5c      �� $ Vertical=�6!   .+�5d      �� $ 
Horizontal=�6%   $��Center=�6>   $��
10,10,0,10q�6Y   .+�5e      �� $��Center=�6)   $	��180��6D   .+�5f   "   ��    RecipientAddress#��Z 6,   $
��Wrap=�6d   $ Center=�6x   $
��Left=�6�   $��Center=�6�   5g      �� 5h      $��Center=�6)   $ 
Horizontal=�6D   .+�5i   "     $
 Wrap=�6-   $	��250��6A   ��6M   �� *
ToEmails+$ TwoWay=��� 5k   "      Send#��Z 6)      ButtonPrimary#��[  6X   $��20,0,0,0q�6    � Cursor$
 Hand��6�   ��6�   !  Stylet.Xaml.ActionExtension  *SendTestEmailCommand+6�   5l      5n      5p      5q      �� 5s         TelegramSettings#��Z 6   $��	10,30,0,0q�6T   $
��Bold"�6g   $��14(�6y   �� 5u      ���	#FFD0D1D7$��	10,0,10,0q�61   $��0,0,0,1q�6D   $
 True��6^   ._�5v      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   $��Stretch=�6i   .+�5w      �� $��Center=�6%   $	��180��6@   .+�5x      ��    TelegramToken#��Z 6(   $
��Wrap=�6]   $
 Left=�6q   $
��Left=�6�   $��Center=�6�   5y      �� 5z      $��Center=�6%   .+�5{        $	��250��6)   $ NoWrap=�65   ��6L   �� *
BotToken+$ TwoWay=�5|      5}      5~      �� 5�      ���	#FFD0D1D7$��	10,0,10,0q�61   $
 True��6E   ._�5�      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   $��Stretch=�6i   .+�5�      �� $��Center=�6%   $	��180��6@   .+�5�      ��    BotAccessList#��Z 6(   $
��Wrap=�6]   $
 Left=�6q   $
��Left=�6�   $��Center=�6�   5�      �� 5�      $��Center=�6%   .+�5�        $	��250��6)   $	��250��68   $
 Wrap=�6D   ��6X   �� *
UserList+$ TwoWay=�5�      5�      5�      5�      5�      � 5�   
   $��1��6   $��0��5�      .+�5�      �� $��Center=�6   $	��240��66   $��0,61,0,0q�6B   .+�5�      �� ���	#FFD0D1D7$	��230��61   $��	10,0,10,0q�6=   ._�5�      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   .+�5�      ]� .+�5�      �� $��0,0,10,0q�6T    d� 	IsChecked 6'   �� *	EnableSSL+$ TwoWay=�6T   �� 5�         SslProtection#��Z 6(   $
��Wrap=�6]   $
 Left=�6q   $
��Left=�6�   $��Center=�6�   5�      5�      5�      �� 5�      ���	#FFD0D1D7$��
10,10,10,0q�61   ._�5�      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   .+�5�      ]� .+�5�      �� $��0,0,10,0q�6`    6'   �� *AuthenticationEnabled+$ TwoWay=�6`   �� 5�         UseAuthentificating#��Z 6(   $
��Wrap=�6c   $
 Left=�6w   $
��Left=�6�   $��Center=�6�   5�      5�      5�      �� 5�      ���	#FFD0D1D7$��
10,10,10,0q�61   $y�	Collapsed=�6E   ._�5�      �� $ 
Horizontal=�6!   $��Center=�6:   $��
10,10,0,10q�6U   .+�5�      ]� .+�5�      �� $��0,0,10,0q�6\    6'   �� *AccidentExpanding+$ TwoWay=�6\   �� 5�         ExpandWindow#��Z 6(   $
��Wrap=�6\   $
 Left=�6p   $
��Left=�6�   $��Center=�6�   5�      5�      5�      5�      5�      5�      5�      