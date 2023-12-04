⎕IO←0
split←{⍺(≠⊆⊢)⍵}
f←{{⍎¨' 'split ⍵}¨'|'split↑1⊃':'split ⍵}¨⊃⎕NGET'../AOCinput'1
sol1←{+/{⌊2*1-⍨≢⊃∩/⍵}¨⍵}
↑{1,≢⊃∩/⍵}¨f