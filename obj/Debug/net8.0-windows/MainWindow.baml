   M S B A M L   `   `   `  ����  TestProject    TestProject.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Faults Simulator��5   	   $
��1600��6"   $	��900��6/   .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� 
����d�$��1q�6.   $��10q�6B   ._�5      �� -   5         ��Name$ motherboardImage��5      $$��Assets\Gigabyte B450M DS3H.png��5      5      5      �� 5   
   -   5         ��Name$ faultsListBox��5      $��1��6)   $��10q�69   $��18(�6w   ��5      �� 5      .R�5      �� ��6    �� *Name+5      5      �� 5   
   -   5         ��Name$ faultDetailsPanel��5      $��1��60   $��10q�6@   $y�	Collapsed=�6L   .+�5       �� $��Fault Details��6   $
��Bold"�6-   $��24(�6?   $��0 0 0 10q�6M   �� 5!      $��0 0 0 10q�6   .+�5"      �� $��Name:��6   $
��Bold"�6)   $��18(�6;   �� 5#      -   5         ��Name$ nameTextBox��5#       �� 
IsReadOnly$
 True��6m   $��18(�6    �� TextWrapping$
 Wrap=�6�   ��6/   �� *SelectedItem.Name+ �� ElementName$ faultsListBox��6�   5$      �� 5%      $��0 0 0 10q�6   .+�5&      �� $��Working:��6   $
��Bold"�6,   $��18(�6>   �� 5'      -   5        $ workingTextBox��5'      $
 True��6   $��18(�6�   $
 Wrap=�6�   ��62   �� *" SelectedItem.Measurement.Working+$ faultsListBox��6�   5(      �� 5)      $��0 0 0 10q�6   .+�5*      �� $��Not working:��6   $
��Bold"�60   $��18(�6B   �� 5+      -   5        $ notWorkingTextBox��5+      $
 True��6�   $��18(�6�   $
 Wrap=�6�   ��65   �� *%#SelectedItem.Measurement.NotWorking+$ faultsListBox��6�   5,      �� 5-      $��0 0 0 10q�6   .+�5.      �� $��	Solution:��6   $
��Bold"�6-   $��18(�6?   �� 5/      -   5        $ solutionTextBox��5/      $
 True��6u   $��18(�6�   $
 Wrap=�6�   ��63   �� *SelectedItem.Solution+$ faultsListBox��6�   50      �� 51      -   5        	 ��Name$	 
backButton��51      $��
Назад��6)   69   52      55      56      