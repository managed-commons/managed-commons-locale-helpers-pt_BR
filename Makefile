# Commons.Locale.Helpers.pt_BR makefile
#
# (c)2005 Rafael Teixeira 
#
# This work is licensed under the Creative Commons Attribution License. 
# To view a copy of this license, visit http://creativecommons.org/licenses/by/2.0/br/ 
# or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.

NAME = Commons.Locale.Helpers.pt_BR

CLEANFILES = Test.exe Test.exe.mdb test.html *.dll

MANAGED_LIBS_NAMES =

TESTTARGET_CSFILES =

CSFILES = \
$(srcdir)/Assembly/AssemblyInfo.cs \
$(srcdir)/Commons.Locale.Helpers.pt_BR/PorExtenso.cs

include ../rules.make

Test.exe: all Test/TestePorExtenso.cs Makefile
	mcs $(MCS_OPTIONS) -target:exe -out:"Test.exe" -r:$(TARGET) Test/TestePorExtenso.cs

test.html: Test.exe
	ln -fs $(TARGET) && mono --debug Test.exe 123.45 1.01 2.99 123456789012345.00 21000000 > test.html 
	
view-test: test.html
	htmlview file://`pwd`/test.html
	
