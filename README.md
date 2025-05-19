# BadBlocksPlaceholder
Places files on HDD's bad blocks. Fill all available space on the selected disk with files and then reads them. 
If the file reads without any errors then it will be deleted. 
If we will get any error it will be left forever on the hard disk in the BadBlockPlaceholders/yyyyMMdd folder.

It accepts two parameters: the disk drive and the block size in KB (file of the file to create). For example:

    BadBlocksPlaceholder e:\ 1024

This will run the test using 1MB files.

Also, you can continue cleaning placeholder files:

    BadBlocksPlaceholder clean e:\BadBlockPlaceholders\20190110

This mode will test all files in the specified folder and delete the files which doesn't have any reading errors.


..............

# This version
Also moves slow files out of the way too.

..............

# Release: BadBlocksPlaceholder-x64-static_v0-01.exe
# WARNING PARTLY AI GENERATED CODE, NO GUARANTEES
Please do take the executable release warnings seriously.
No guarantees or warranties of any kind.
