# About
What is BON? BON is binary object notation, more compact alternative for JSON and BSON. 
It, as the name implies, converts object to byte array. You can create yourself converters for custom types.
# How does it works
This tecnhology doesn't use names or special symbols, saving value of object whether it's array, any number, class, struct, etc.<br/>
Actually it uses three base types: **byte array**, **fixed byte array** and **nullable**.
End result is concatanation of blocks of those three types. 
## Byte array
This is classical byte array, but with its size before first element
## Fixed byte array
This is class, struct, int or anymore with fixed size representation. In contrast to **byte array**, this doesn't contain length at begining.
You can know length of fixed byte array by serializing type and by byte array if fixed byte array contains it.
## Nullable
Nullable sign that value is null or not. If value is null, then first byte of block is 0, if not null 1.


