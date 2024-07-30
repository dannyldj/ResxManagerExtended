// noinspection JSUnusedGlobalSymbols

export async function getRootDirectory() {
    try {
        // noinspection JSUnresolvedReference
        return await window.showDirectoryPicker();
    } catch (ex) {
        return null;
    }
}

export function getHandlerName(handler) {
    return handler.name;
}

export async function getResourceFiles(handler) {
    return (await (await handler.values()).next()).done ? null : getFiles(handler, undefined, "resx");
}

const getFiles = async (
    dirHandle,
    path = dirHandle.name,
    ...extensions
) => {
    const dirs = [];
    const files = [];
    for await (const entry of dirHandle.values()) {
        const nestedPath = `${path}/${entry.name}`;
        if (entry.kind === 'file' && extensions.includes(entry.name.split('.').pop())) {
            files.push(
                entry.getFile().then((file) => {
                    file.directoryHandle = dirHandle;
                    file.handle = entry;
                    return Object.defineProperty(file, 'webkitRelativePath', {
                        configurable: true,
                        enumerable: true,
                        get: () => nestedPath,
                    });
                })
            );
        } else if (entry.kind === 'directory') {
            dirs.push(getFiles(entry, nestedPath));
        }
    }
    return [...(await Promise.all(dirs)).flat(), ...(await Promise.all(files))];
};