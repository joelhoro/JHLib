def pointwiseoperation2(operator):
   def newoperator(a,b):
      if (not isinstance(a,V) and not isinstance(b,V)):
         return operator(a,b)
      if (not isinstance(a,V)):
         a = b.constant(a)
      if (not isinstance(b,V)):
         b = a.constant(b)
      c = a.constant()
      for i,(v,w) in enumerate(zip(a.v,b.v)):
         c.v[i] = operator(v,w)
      return c
   return newoperator

def pointwiseoperation(operator):
   def newoperator(a):
      if (not isinstance(a,V)):
         return operator(a)
      c = a.constant()
      for i,v in enumerate(a.v):
         c.v[i] = operator(v)
      return c
   return newoperator

class V():
   def __init__(self,v):
      self.v = v.Clone()
   def __add__(self,other):
      return V(self.v.Add(other))
   __radd__ = __add__

   def __sub__(self,other):
      return V(self.v.Subtract(other))

   def __mul__(self,other):
      return V(self.v.Multiply(other))
   __rmul__ = __mul__

   def __div__(self,other):
      if isinstance(other,V):
         return V(self.v.PointwiseDivide(other.v))
      else:
         return V(self.v.Divide(other))
   def __rdiv__(self,other):
      if isinstance(other,V):
         return V(other.v.PointwiseDivide(self.v))
      else:
         return self.constant(other) / self

   def constant(self,value=0):
      return self*0+value

   max = staticmethod(pointwiseoperation2(max))
   min = staticmethod(pointwiseoperation2(min))
   abs = staticmethod(pointwiseoperation(abs))

   def __str__(self):
      return "V[" + str(self.v).replace("\r\n"," ") + "]"
   __repr__ = __str__


def Test(v):
   newv = V(v)
   print "Vector: ", newv
   print "Vector + 10: ", newv + 10
   print "Vector * 10: ", newv * 10
   print "Vector / 10: ", newv / 10
   print "V1 / V2", ( newv + 10 ) / ( newv - 10 )
   print "1000 / Vector", 1000 / newv


def TestMax(v):
	w = newv-30
	w *= 1.35
	print newv
	print w
	print
	print V.max(newv,w)
	print V.max(newv,100)
	print V.max(100,newv)
	print V.max(100,110)
	print V.min(newv,w)
	print V.min(newv,100)
	print V.min(100,newv)
	print V.min(100,110)
	print
	print newv-104
	print V.abs(newv-104)
