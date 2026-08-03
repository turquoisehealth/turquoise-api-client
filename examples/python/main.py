from turquoise_health import TurquoiseHealth
from turquoise_health.lib import APIAuthHandler
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

# Create the auth handler once and reuse it — it caches the OAuth token in
# memory and refreshes it automatically. Building a new one per request
# bypasses that cache and can trip rate limits.
auth = APIAuthHandler.from_client_credentials()

client = TurquoiseHealth(
    base_url="https://api.turquoise.health",
    token=auth.as_callable()
)


def main():
    # Search for shoppable service packages by name.
    packages = client.consumer_pricing.v3list_packages(search="MRI Brain")

    if not packages.items:
        print("No matching service packages found.")
        return

    ssp = packages.items[0]
    print(f"Found package: {ssp.name} ({ssp.id})")

    # Get negotiated prices for that package near a given zip code / network.
    prices = client.consumer_pricing.v3query_prices(
        package_id=ssp.id,
        pricing={
            "type": "negotiated",
            "network_id": "-3776001016975145508",
        },
        location={"zip": "90210"},
    )

    for price in prices.items:
        print(price.provider.name, price.total.amount)


if __name__ == "__main__":
    try:
        main()
    except Exception as err:
        print(f"Error: {err}")
        exit(1)
