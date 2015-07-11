import sys
sys.path.append("..\..\lib")

import code

def startconsole(objects):
	vars = globals()
	vars.update(locals())
	shell = code.InteractiveConsole(vars)
	print "=" * 80 + "\n"
	print "Try exploring the 'objects' variable in the interactive prompt\n"
	print "or something like this: [ objects['C'].Multiplier(i) for i in range(5) ]\n"
	print "objects = ", objects
	print "=" * 80 + "\n"
	shell.interact()
	print "Finished"