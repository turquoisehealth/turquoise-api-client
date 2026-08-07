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
