# Python example api client project starter

Example project using [`turquoisehealth-api`](https://pypi.org/project/turquoisehealth-api/).

## Setup

```bash
python3 -m venv venv
source venv/bin/activate
pip install -r requirements.txt
cp .env.example .env
# fill in TURQUOISE_CLIENT_ID / TURQUOISE_CLIENT_SECRET / TURQUOISE_ORGANIZATION_ID in .env
```

## Run

```bash
python main.py
```

Runs [main.py](main.py), which searches for a service package and lists prices for it.

## Use the local SDK

To use the SDK source in this repository instead of the published PyPI package,
activate your virtual environment and run these commands from `examples/python`:

```bash
python -m pip install ../../python python-dotenv
python main.py
```

For a fresh environment, follow the setup steps above but replace
`pip install -r requirements.txt` with the local install command. Configure `.env`
as usual.

The install command builds and installs the local SDK. Rerun it after changing
SDK source files; changes are not picked up automatically. Editable installs
(`pip install -e ../../python`) are not supported by the current package layout.

To confirm the installed SDK came from this checkout:

```bash
python -m pip show turquoisehealth-api
python -c "from importlib.metadata import distribution; print(distribution('turquoisehealth-api').read_text('direct_url.json'))"
```

The second command should show a local `file://` URL ending in this repository's
`python` directory.
