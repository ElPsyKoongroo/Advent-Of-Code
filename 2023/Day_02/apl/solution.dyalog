⎕IO←0
f←⊃⎕NGET'../AOCinput'1
f1←(','(≠⊆⊢)¨';'(≠⊆⊢)1⊃':'(≠⊆⊢)⊢)¨f
fC←{∊12+⍸¨↓⍉(+/¨c∘.⍷⊢)⍵}
getN←{(10⊥¯1~⍨1-⍨11|1+⎕D∘⍳¨)¨⍵}
c←' '(≠⊆⊢)'red green blue'

sol1←{+/1+⍸{~∨/∨/∘(fC<getN)¨⍵}¨⍵}
sol2←{+/{×/(∊fC¨⍵){⊃⍵[⊃⍒⍵]}⌸∊getN¨⍵}¨⍵}