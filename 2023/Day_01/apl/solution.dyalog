⎕IO←0
input ← ⊃⎕NGET'../AOCinput'1
numbers←('one' 'two' 'three' 'four' 'five' 'six' 'seven' 'eight' 'nine')

getNums ← {10|⎕D∘⍳¨⍵}
getNum ← {10⊥∊1 ¯1↑¨⊂⍵~0}

⍝ sol1←{+/{⍎(⊃,⊃∘⌽)(⍵∊⎕D)/⍵}¨⍵}
sol1←{+/getNum¨getNums⍵}
sol2 ← {+/getNum¨(getNums⍵)+(1+⍳9)+.×numbers∘.⍷⍵}
