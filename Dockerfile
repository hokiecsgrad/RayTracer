FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install essential tools
RUN apt-get update && apt-get install -y \
    curl \
    procps \
    unzip \
    zip \
    openssh-client \
    && rm -rf /var/lib/apt/lists/*

# Install global .NET tools
RUN dotnet tool install -g dotnet-format

# Add .NET tools to PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Create a non-root user to use if preferred - same UID/GID as default WSL/Ubuntu user
RUN groupadd --gid 1000 vscode \
    && useradd --uid 1000 --gid 1000 -m vscode \
    && chown -R vscode:vscode /home/vscode

# Set up working directory
WORKDIR /workspace

# Set the default user to use (uncomment if you prefer to run as non-root)
# USER vscode

# Keep container alive
CMD ["sleep", "infinity"]