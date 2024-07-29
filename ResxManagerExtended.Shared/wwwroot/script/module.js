export async function getRootDirectory() {
    // noinspection JSUnresolvedReference
    return await window.showDirectoryPicker();
}