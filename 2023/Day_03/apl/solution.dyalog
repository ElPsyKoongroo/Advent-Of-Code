⎕IO←0
f←⊃⎕NGET'../AOCtest'1

f←⊃⎕NGET'../AOCinput'1

getN←{(×11|1+⎕D∘⍳¨)¨⍵}
getP←{('.'=⊢)¨⍵}

getS←{~(getN+getP)⍵}
getNS←{(↑getN ⍵)+(2×(↑getS ⍵))}
getNS2←{(↑getN ⍵)+(2×('*'=↑⍵))}

getNear←{({(2=⌈/),⍵}⌺3 3)getNS ⍵}
getNear2←{({(2=⌈/),⍵}⌺3 3)getNS2 ⍵}
getNear3←{({(2=⌈/),⍵}⌺3 3) ⍵}

getTouch←{(↑getN ⍵)∧(↑getNear ⍵)}
getTouch2←{(↑getN ⍵)∧(↑getNear2 ⍵)}
getTouch3←{(↑getN ⍵)∧(↑getNear3 remBad ⍵)}

splitZ←{2=∊{(≢⍵)⍴(⌈/⍵)}¨(1,1↓((⊢≠¯1⌽⊢)(×⍵)))⊂⍵}
goodNums←{↑splitZ¨↓(↑getN ⍵)+(getTouch ⍵)}
goodNums2←{↑splitZ¨↓(↑getN ⍵)+(getTouch3 ⍵)}

getUnique←{1=↑{∊+\¨(1,1↓((⊢≠¯1⌽⊢)⍵))⊂⍵}¨↓(getTouch2 ⍵)}
remBad←{({1⌷1⌷⍵≠2:(1⌷1⌷⍵)⋄2×(2=+/+/1=⍵)}⌺3 3)(getUnique ⍵) + 2×('*'=↑⍵)}

finalNums←{⍎¨(∊↑goodNums2 ⍵)⊆∊↑⍵}
finalIndexes←{1=(2×2=(remBad ⍵))+((remBad ⍵)∧goodNums2 ⍵)}
finalMatrix←{1@(=∘0)(¯1×2=(remBad ⍵))+(finalNums ⍵)@(=∘1)(finalIndexes ⍵)}

sol1←{+/⍎¨(∊↑goodNums ⍵)⊆∊↑⍵}
sol2 ←{+/∊({(1⌷1⌷⍵)≠¯1:0⋄(-×/∊⍵)}⌺3 3)finalMatrix ⍵}

