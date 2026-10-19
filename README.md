# RPGFramework.Hashing
Small package to handle Fnv1a64 hashing

## Tests

The tests are in `Tests/Editor`, on Unity's Test Framework. They compile only in a project that lists the package as
testable: add it to `testables` in `Packages/manifest.json`,

```json
"testables": [
  "com.rpgframework.hashing"
]
```

then open **Window › General › Test Runner** and run the **EditMode** tests.
