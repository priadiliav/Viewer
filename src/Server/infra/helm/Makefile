SHELL := cmd.exe
.SHELLFLAGS := /C

ENV ?= default
RELEASE_NAME = viewer
CHART_PATH = ./
VALUES_FILE = values-local.yaml

# pfx
# kubectl apply -k "https://github.com/kubernetes-sigs/gateway-api/config/crd?ref=v1.3.0"

deploy:
	@helm list -q | findstr $(RELEASE_NAME) >nul 2>&1 && ( \
		echo Upgrading release $(RELEASE_NAME)... && \
		helm upgrade $(RELEASE_NAME) $(CHART_PATH) -f $(VALUES_FILE) \
	) || ( \
		echo Installing release $(RELEASE_NAME)... && \
		helm install $(RELEASE_NAME) $(CHART_PATH) -f $(VALUES_FILE) \
	)

delete:
	@helm uninstall $(RELEASE_NAME) || echo Release $(RELEASE_NAME) not found or already deleted.
