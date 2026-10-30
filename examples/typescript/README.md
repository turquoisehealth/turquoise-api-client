# Typescript example api client project starter

Example project using [`@turquoisehealth/api`](https://www.npmjs.com/package/@turquoisehealth/api).

## Setup

```bash
npm install
cp .env.example .env
# fill in TURQUOISE_CLIENT_ID / TURQUOISE_CLIENT_SECRET / TURQUOISE_ORGANIZATION_ID in .env
```

## Run

```bash
npm start
```

Runs [src/index.ts](src/index.ts), which searches for a service package and lists prices for it.

## Use the local SDK

From `examples/typescript`, install the local SDK's build dependencies once:

```bash
npm --prefix ../../typescript ci
```

Then build and link the local SDK and run the example:

```bash
npm run sdk:local
npm start
```

The `sdk:local` command compiles the SDK in
`../../typescript` and installs it as a local link without changing the published
dependency in `package.json` or the lockfile. Rerun `npm run sdk:local` after
changing SDK source files, then restart the example.

Validate what the example is using with

```bash
npm ls @turquoisehealth/api
```

Switch back to the published package specified in package.json with:

```bash
npm install
```
