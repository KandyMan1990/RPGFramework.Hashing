# RPGFramework.Hashing

FNV-1a 64-bit hashing of text and bytes, small and allocation-conscious. The RPG Framework names things by these
hashes — save and settings sections, localisation keys, and the songs, sounds, fields and transitions content refers to
by name — so a name hashes to the same value on every platform and in every build.

Requires Unity 6000.0 or newer, and depends on nothing else. The runtime assembly has no engine references: it is plain
C#.

---

## Installation

Install through the Package Manager from the repository's latest release tag.

---

## Usage

`Fnv1a64.Hash` has four overloads: a string, a span of characters, a byte array and a span of bytes.

```csharp
using System;
using RPGFramework.Hashing;

ulong section = Fnv1a64.Hash("RPGFramework.PersistentMemory");

string key   = "Menu/Begin";
ulong  sheet = Fnv1a64.Hash(key.AsSpan(0, 4)); // "Menu", without allocating a substring

byte[] bytes = { 0x01, 0x02, 0x03 };
ulong  data  = Fnv1a64.Hash(bytes);
```

### What is hashed

- **Text is hashed as UTF-8**, so a string hashes the same as its UTF-8 bytes, and a span the same as the string it
  covers.
- **Null is empty**: a null string or array hashes as an empty one, as `((string)null).AsSpan()` is empty.
- **Whitespace is ordinary input**: `" "` and `""` hash differently.
- **Malformed text does not throw**: an unpaired surrogate is encoded as the replacement character, U+FFFD, so two
  differently malformed strings can hash alike.
- **No value is reserved**: empty input returns the FNV-1a offset basis, and neither that nor `0` can mean "absent".
  Reject null, empty or whitespace keys before hashing if they are not valid for your data.
- **Text is not normalised**: an accented letter written as one character and as a letter and a combining accent hash
  differently, as do different cases.
- **Short text allocates nothing**: up to 85 characters are encoded on the stack, longer text in a pooled buffer.

### Hashes are stored

A hash ends up in saves and content files, so the values must never change: the algorithm and the UTF-8 encoding are
fixed, and a game that changed them would orphan every save section and localisation key it has written.

---

## Tests

The tests are in `Tests/Editor`, on Unity's Test Framework. They compile only in a project that lists the package as
testable: add it to `testables` in `Packages/manifest.json`,

```json
"testables": [
  "com.rpgframework.hashing"
]
```

then open **Window › General › Test Runner** and run the **EditMode** tests.

---

## In this version

**2.0.2** adds the tests: the algorithm's published test vectors, text against its UTF-8 bytes, null and whitespace, an
unpaired surrogate, and both sides of the 85-character limit. The API and every hash value are unchanged.

---

## Not in this version

- **Seeded or chained hashing**: one was added and removed again, as nothing needed it.
- **Other algorithms or widths**: FNV-1a 64 only.
- **Normalisation**: text is hashed as given; normalise it first if two spellings of a name must match.
