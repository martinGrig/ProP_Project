   M S B A M L   `   `   `  ���� G  CEventManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EventManager.ViewsEventManager.Views  A%clr-namespace:EventManager.ViewModelsEventManager.ViewModels  "    EventManager.Views.LoginView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     2local clr-namespace:EventManager.Views     <
viewmodels%clr-namespace:EventManager.ViewModels       � Triggers  :�  :� RoutedEvent$ Window.Loaded��.[��� .���� .��W�  g� 	BeginTime$ 0:0:0X� �� 
TargetName$
 Logo�� �� TargetProperty$ Opacity� W� From$	 0.0X�	 W� To$	 1.0X� g� Duration$ 0:0:1D� g� AutoReverse . W� $ 0:0:0X�$
 Logo��$4 .(Image.RenderTransform).(TranslateTransform.Y)�		 W� By$
	 -437X�$ 0:0:0.5D�W� -   $ 0:0:0X�$ TempBack��$ Opacity�$	 1.0X�$	 0.0X�$ 0:0:2D�.��� .+�(� .���� 
 �� Padding$
 0q�._��� $��0��$	��300��$��Center=�$��10q�$��Center=�$��Log-In��   FirstGroupbox#��[  $��2��$��2��.���  � ColumnDefinitions �� $��* ���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��Employee Number��$��0��$��0��  
HeaderText#��[ �� $��Password��$��1��$��0��#��[ �� $��1��$��0��$��28��  NormalTextBox#��[ ���� *Dm.EmployeeNumber+ �� UpdateSourceTrigger$ PropertyChanged=�G� -    G�Name$ passwordBox��$��1��$��Center=�$��1��  PasswordBox#��[ �� $��Log-In��$��0��$��2��$��Stretch=�$��2��  
ButtonTest#��[ $��10q�$��50������ *Click_LoginCommand+����  �� ElementName$ passwordBox���� $��Forgot your password?��$��0��$��2��$��3��$��Center=�$��	10 0 0 15q�  ForegroundMainBrush#��[ �� -   $��TempBack��
�������$y�	Collapsed=��� -   $
��Logo��$.��(/EventManager;component/Images/MLogo.png��z�T�  T� Y$	 437Q�