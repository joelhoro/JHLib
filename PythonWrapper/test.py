import sys
import clr
path = 'c:\\Users\\joel\\Documents\\Visual Studio 2013\\Projects\\JHLib\\Ironpython\\Ironpython\\bin\\Debug'
sys.path.append(path)
#path = path + "\\" + "MysClass.dll"
print path
clr.AddReferenceToFile( "MyClass.dll" )

import MyClass

print "Loading test.py"
for p in [ #"C:\Users\joel\AppData\Local\Enthought\Canopy\User\Lib\site-packages", 
		"c:\Program Files (x86)\IronPython 2.7\Lib", "c:\Program Files (x86)\IronPython 2.7\Lib\site-packages" ]:
	#print "Appending this to path: ", p
	sys.path.append(p)
	
import random


CALL = "CALL"
PUT = "PUT"
BINCALL = "BINCALL"
BINPUT = "BINPUT"

def option(K,optiontype):
	optiontype = str(optiontype)
	if optiontype == "CALL":
		return lambda S: max(S-K,0)
	if optiontype == "PUT":
		return lambda S: max(K-S,0)
	if optiontype == "BINCALL":
		return lambda S:int(S>=K)			
	if optiontype == "BINPUT":
		return lambda S:int(S<=K)			


def path():
	print sys.path


def arrayfn(x):
	print x
	return x

def dictfn(x):
	return { 'a' : { 'b' : 123 }, 'b' : { 'c' : 456 }, 'c' : 45.3 }


def logfn(f):
	print "Creating the decorator"
	def logf(*args):
		print "Running decorated f"
		f(*args)
		print "Finished running decorated f"
	return logf

def fntolog(x):
	print "Getting ", x
	print "Returning ", 2*x
	return 2*x


tableau = [

{ 'Date' : "Date('5May14')",		'S' : "Spot('MSFT')",		'Ret' : "0",					'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5Aug14')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5Nov14')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5Feb15')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5May15')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5Aug15')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5Nov15')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5Feb16')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },
{ 'Date' : "Date('5May16')",		'S' : "Spot('MSFT')",		'Ret' : "S[i]/S[i-1]-1",		'Payoff' : "Max(0,Min(0.05,Ret[i]))"    },

]

def objecttest(ob):
	print dir(ob)
	ob.test()

def square(x):
	return x*x
