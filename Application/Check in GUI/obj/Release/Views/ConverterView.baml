   M S B A M L   `   `   `  ���� G  CEventManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:EventManager.ConvertersEventManager.Converters  7 clr-namespace:EventManager.ViewsEventManager.Views  &     EventManager.Views.ConverterView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     <
converters%clr-namespace:EventManager.Converters     2local clr-namespace:EventManager.Views       � 	Resources      booleanToVisibilityConverter3  -EventManager.Converters.LogFileRegExConverter  RegExConverter  _ListBoxItemStyle �� 
TargetType �� Property �� Value �� 
TargetType �� Padding J� SnapsToDevicePixels �� Triggers P� Property	 P� Value
 �� 
TargetName��%\  &	        &	      &	 
     ��   �� " ��.���  � {� �� *	IsEnabled+��  � ��$ 0,0.25q���  � �� �� " ��.R��� $��_Border��$ 2q� . ._���  P�  �@��
IsSelected	 . . ��� $
 _Border�� � ��
 �P����  � ��
 �����P�  � {�	 .  . ��� $
 _Border�� � ��
 �  ��.��(� $
��Auto��.���� $	��400��$	��400��$��10q�._��� $ ��Transaction File Converter��  ThirdGroupbox#��[ .���  � ColumnDefinitions �� ��  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+���   
HeaderText#��[ $ ��Load transaction-log-file:��$��20q���   
ButtonTest#��[ $��1��$��0��$
��Load��$��10 10 10 10q����� *LoadFileCommand+��   ReadOnlyTextBox#��[ $��1��$��2������ *LogFileTitle+�� $��2��$��2��$��10q� �� SelectionMode$ Multiple=� �� DisplayMemberPath$
 Line��#���  $
��Auto=�$
��Auto=����� *'%_mainViewModel.dataModel.LogFileLines+��  �� Orientation$ 
Horizontal=�$��0��$��3��$��2��$��Center=�.+��� #��[ $��40��$��Upload Selected��$��10 10 10 10q��� #��[ $��Ignore Selected��$��10 10 10 10q��� #��[ $��Upload Everything��$��10 10 10 10q�