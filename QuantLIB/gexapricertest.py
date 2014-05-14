from vector import V
import clr, sys

from System import DateTime
import System

sys.path.append(r"c:\Users\joel\Documents\Python\graph")
from gexapricer import Cliquet, today

path = r"c:\Users\joel\Documents\Visual Studio 2013\Projects\JHLib\QuantLib\bin\debug" + "\\"
clr.AddReferenceToFileAndPath(path + "QL.dll")

from JHLib.QuantLIB import *


class GexaModel():
	def __init__(self,equity):
		self.model = EquityModel(equity = equity)
	def spot(self,date):
		maturity = ( date - today() ) / 365.
		v=V(self.model.Spot(maturity))
		print today(), "-->", date, "(", maturity, "y)", v
		return v


equity = Equity( spot=100, sigma = 0.2 )

d = DateTime(2015,1,1)
fn = lambda: equity.Variance(d)

cliquet = Cliquet(start_date = today() + 1, periods = 4)
gexamodel = GexaModel(equity)
#System.Diagnostics.Debugger.Launch()

#print cliquet.Paths(gexamodel)

tableau = cliquet.raw_tableau()
columns = tableau[0]
for (i,row) in enumerate(tableau[0:]):
	for (j,col) in enumerate(columns):
		print "Column " + col + " / row=" + str(i)
		print "================================"
		print "Formula = ", row[j]
		value = cliquet.tableau_cell(i-1,col,model=gexamodel)
		print "\nReturn value=", value
		print "\n\n"
            #print "Evaluation for ", col, " / ", str(i)
	    #print "=========================="
            #print output
            #print 



